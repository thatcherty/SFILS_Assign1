# app/streamlit_app.py
import os
from datetime import date, timedelta

import pandas as pd
import streamlit as st
from sqlalchemy import create_engine, text

# ---- Config ----
st.set_page_config(page_title="SFILS Findings", layout="wide")
st.title("SFILS: Results & Findings")

DB_USER = os.getenv("DB_USER", "root")
DB_PASS = os.getenv("DB_PASS", "rootPassword123!")
DB_HOST = os.getenv("DB_HOST", "localhost")
DB_NAME = os.getenv("DB_NAME", "sfils")

# ---- DB engine (cached once) ----
@st.cache_resource
def get_engine():
    # pip install pymysql sqlalchemy
    return create_engine(f"mysql+pymysql://{DB_USER}:{DB_PASS}@{DB_HOST}/{DB_NAME}")

engine = get_engine()

# ---- Helper: run SQL safely into pandas ----
@st.cache_data(show_spinner=False, ttl=300)
def read_df(sql: str, params: dict = None) -> pd.DataFrame:
    with engine.connect() as conn:
        return pd.read_sql(text(sql), conn, params=params or {})

# ---- Sidebar Filters ----
st.sidebar.header("Filters")

# Date range defaults: last 180 days
end_default = date.today()
start_default = end_default - timedelta(days=180)
start_date, end_date = st.sidebar.date_input(
    "Checkout date range",
    value=(start_default, end_default),
    help="Filters by c.checkout_date"
)

# Branch list
branches_df = read_df("SELECT branch_id, name FROM branches ORDER BY name;")
branch_names = branches_df["name"].tolist()
selected_branches = st.sidebar.multiselect("Branches", branch_names)

min_borrows = st.sidebar.slider("Minimum borrows (for Top Books)", 0, 500, 10, 5)

col_btn1, col_btn2 = st.sidebar.columns(2)
refresh_clicked = col_btn1.button("🔁 Refresh data")
# Clear caches if user clicks refresh
if refresh_clicked:
    read_df.clear()

# ---- Queries (server-side) ----
# Top books (server-side aggregated), branch filter applied in pandas for simplicity
top_books_sql = """
SELECT b.title, br.name AS branch, COUNT(*) AS borrows
FROM checkouts c
JOIN books b     ON b.book_id = c.book_id
JOIN branches br ON br.branch_id = c.branch_id
WHERE c.checkout_date BETWEEN :start AND :end
GROUP BY b.title, br.name
;
"""
top_books = read_df(top_books_sql, {"start": start_date, "end": end_date})

# Monthly trend
monthly_sql = """
SELECT DATE_FORMAT(c.checkout_date, '%Y-%m') AS month, COUNT(*) AS borrows
FROM checkouts c
WHERE c.checkout_date BETWEEN :start AND :end
GROUP BY month
ORDER BY month;
"""
monthly = read_df(monthly_sql, {"start": start_date, "end": end_date})

# By branch
by_branch_sql = """
SELECT br.name AS branch, COUNT(*) AS borrows
FROM checkouts c
JOIN branches br ON br.branch_id = c.branch_id
WHERE c.checkout_date BETWEEN :start AND :end
GROUP BY br.name
ORDER BY borrows DESC;
"""
by_branch = read_df(by_branch_sql, {"start": start_date, "end": end_date})

# ---- Apply pandas-side filters ----
if selected_branches:
    top_books = top_books[top_books["branch"].isin(selected_branches)]
    by_branch = by_branch[by_branch["branch"].isin(selected_branches)]

if min_borrows:
    # Keep only titles that meet min total across selected branches
    title_totals = top_books.groupby("title", as_index=False)["borrows"].sum()
    keep_titles = title_totals[title_totals["borrows"] >= min_borrows]["title"]
    top_books = top_books[top_books["title"].isin(keep_titles)]

# ---- Layout ----
c1, c2 = st.columns(2, gap="large")

with c1:
    st.subheader(f"Top Borrowed Books ({start_date} → {end_date})")
    # Aggregate across branches for chart clarity
    tb_chart = (top_books.groupby("title", as_index=False)["borrows"].sum()
                .sort_values("borrows", ascending=False).head(15))
    if not tb_chart.empty:
        st.bar_chart(tb_chart.set_index("title"))
    else:
        st.info("No data for selected filters.")

with c2:
    st.subheader(f"Monthly Borrowing Trend ({start_date} → {end_date})")
    if not monthly.empty:
        st.line_chart(monthly.set_index("month"))
    else:
        st.info("No data for selected range.")

st.subheader("Borrowing by Branch")
if not by_branch.empty:
    st.bar_chart(by_branch.set_index("branch"))
else:
    st.info("No branch data for current filters.")

st.subheader("Raw Tables")
st.dataframe(top_books.reset_index(drop=True), use_container_width=True)
st.dataframe(monthly.reset_index(drop=True), use_container_width=True)
st.dataframe(by_branch.reset_index(drop=True), use_container_width=True)

# ---- Export buttons ----
export_col1, export_col2, export_col3 = st.columns(3)
export_col1.download_button(
    "⬇️ Export Top Books (CSV)",
    top_books.to_csv(index=False).encode("utf-8"),
    file_name="top_books.csv",
    mime="text/csv"
)
export_col2.download_button(
    "⬇️ Export Monthly (CSV)",
    monthly.to_csv(index=False).encode("utf-8"),
    file_name="monthly.csv",
    mime="text/csv"
)
export_col3.download_button(
    "⬇️ Export By Branch (CSV)",
    by_branch.to_csv(index=False).encode("utf-8"),
    file_name="by_branch.csv",
    mime="text/csv"
)
