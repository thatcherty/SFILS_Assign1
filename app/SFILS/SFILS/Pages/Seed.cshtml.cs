using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;

namespace SFILS.Pages
{
    public class SeedModel : PageModel
    {
        private readonly IMongoDatabase _db;
        private readonly IWebHostEnvironment _env;

        // Track collections, validator file, and data file
        private readonly Dictionary<string, (string ValidatorFile, string DataFile)> _collections =
            new()
            {
                // collectionName          validator file                       data file
                ["age_ranges"] = ("age_ranges.validator.json", "age_ranges.data.json"),
                ["home_libraries"] = ("home_libraries.validator.json", "home_libraries.data.json"),
                ["notification_pref"] = ("notification_pref.validator.json", "notification_pref.data.json"),
                ["patron_types"] = ("patron_types.validator.json", "patron_types.data.json"),
                ["patrons"] = ("patrons.validator.json", "patrons.data.json"),
            };

        public string? ResultMessage { get; set; }
        public bool Ran { get; set; }
        public bool HasError { get; set; }

        public SeedModel(IMongoDatabase db, IWebHostEnvironment env)
        {
            _db = db;
            _env = env;
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                await SeedAsync();
                Ran = true;
                HasError = false;
                ResultMessage = "MongoDB SFILS database initialized successfully.";
            }
            catch (Exception ex)
            {
                Ran = true;
                HasError = true;
                ResultMessage = $"Error during seeding: {ex.Message}";
            }

            return Page();
        }

        private async Task SeedAsync()
        {
            await EnsureCollectionsExistAsync();
            await ApplyValidatorsAsync();
            await SeedDataAsync();
        }

        private async Task EnsureCollectionsExistAsync()
        {
            var existing = await _db.ListCollectionNames().ToListAsync();

            foreach (var kvp in _collections)
            {
                var collectionName = kvp.Key;

                if (!existing.Contains(collectionName))
                {
                    await _db.CreateCollectionAsync(collectionName);
                }
            }
        }

        private async Task ApplyValidatorsAsync()
        {
            foreach (var kvp in _collections)
            {
                var collectionName = kvp.Key;
                var validatorFile = kvp.Value.ValidatorFile;

                await ApplyValidatorFromFileAsync(validatorFile, collectionName);
            }
        }

        private async Task ApplyValidatorFromFileAsync(string validatorFileName, string collectionName)
        {
            var path = Path.Combine(
                _env.ContentRootPath,
                "seed_data",
                "mongo",
                "validators",
                validatorFileName);

            if (!System.IO.File.Exists(path))
            {
                // You can throw here if you want this to be fatal
                return;
            }

            var json = await System.IO.File.ReadAllTextAsync(path);
            if (string.IsNullOrWhiteSpace(json))
            {
                return;
            }

            // JSON is the body of the command: { "collMod": "...", "validator": { ... } }
            var commandDoc = BsonSerializer.Deserialize<BsonDocument>(json);

            // Safety: make sure collMod matches the collection name
            commandDoc["collMod"] = collectionName;

            await _db.RunCommandAsync<BsonDocument>(commandDoc);
        }

        private async Task SeedDataAsync()
        {
            foreach (var kvp in _collections)
            {
                var collectionName = kvp.Key;
                var dataFile = kvp.Value.DataFile;

                await SeedCollectionFromFileAsync(collectionName, dataFile);
            }
        }

        private async Task SeedCollectionFromFileAsync(string collectionName, string dataFileName)
        {
            var path = Path.Combine(
                _env.ContentRootPath,
                "seed_data",
                "mongo",
                "data",
                dataFileName);

            if (!System.IO.File.Exists(path))
            {
                // Optional: log or ignore
                return;
            }

            var collection = _db.GetCollection<BsonDocument>(collectionName);

            // Prevent reseeding if data already exists
            var existingCount = await collection.EstimatedDocumentCountAsync();
            if (existingCount > 0)
            {
                return;
            }

            var json = await System.IO.File.ReadAllTextAsync(path);
            if (string.IsNullOrWhiteSpace(json))
            {
                return;
            }

            List<BsonDocument> docs;

            try
            {
                // Expect the file to be a JSON array: [ { ... }, { ... } ]
                docs = BsonSerializer.Deserialize<List<BsonDocument>>(json);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(
                    $"Failed to deserialize data file '{dataFileName}'. Ensure it contains a JSON array.",
                    ex);
            }

            if (docs.Count == 0)
            {
                return;
            }

            await collection.InsertManyAsync(docs);
        }
    }
}
