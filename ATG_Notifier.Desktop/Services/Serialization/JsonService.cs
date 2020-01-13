using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace ATG_Notifier.Desktop.Services.Serialization
{
    internal class JsonService
    {
        /// <summary>The JSON settings to use when serializing/deserializing files.</summary>
        public JsonSerializerOptions JsonOptions { get; } = new JsonSerializerOptions
        {
            WriteIndented = true,
        };

        /// <summary>
        /// Read a JSON file asynchronously.
        /// </summary>
        /// <typeparam name="TModel">The model type.</typeparam>
        /// <param name="fullPath">The absolete file path.</param>
        /// <returns>The parsed model on success, otherwise a default representation of <typeparam name="TModel">.</returns>
        /// <exception cref="ArgumentException">The given <paramref name="fullPath"/> is empty or invalid.</exception>
        /// <exception cref="JsonException">The file contains invalid JSON.</exception>
        public async ValueTask<TModel?> ReadJsonFileAsync<TModel>(string fullPath)
            where TModel : class
        {
            if (string.IsNullOrWhiteSpace(fullPath))
            {
                throw new ArgumentException("The file path is empty or invalid.", nameof(fullPath));
            }

            // Open the file for reading. If the file doesn't exist, we return a default representation
            // for the specified target type.
            FileStream fileStream;
            try
            {
                fileStream = File.OpenRead(fullPath);
            }
            catch (Exception ex) when (ex is DirectoryNotFoundException || ex is FileNotFoundException)
            {
                return default;
            }

            // Deserialize the opened file into an instance of the specified target type.
            TModel model;
            try
            {
                model = await JsonSerializer.DeserializeAsync<TModel>(fileStream, JsonOptions);
            }
            catch (Exception ex)
            {
                string error = $"Cannot parse JSON file at {fullPath}.";

                if (ex is JsonException)
                {
                    error += " This doesn't seem to be valid JSON.";
                }

                error += $"\nTechnical details: {ex.Message}";
                throw new JsonException(error);
            }

            // cleanup any resources used
            await fileStream.DisposeAsync();

            return model;
        }

        /// <summary>
        /// Save a representation of the specified model to a JSON file.
        /// </summary>
        /// <typeparam name="TModel">The model type.</typeparam>
        /// <param name="fullPath">The absolete file path.</param>
        /// <param name="model">The model to save.</param>
        /// <exception cref="ArgumentException">The given path is empty or invalid.</exception>
        public void WriteJsonFile<TModel>(string fullPath, TModel model)
            where TModel : class
        {
            // validate the specified path for correctness
            if (string.IsNullOrWhiteSpace(fullPath))
            {
                throw new ArgumentException("The file path is empty or invalid.", nameof(fullPath));
            }

            // create file directory if needed
            string? dir = Path.GetDirectoryName(fullPath);
            if (dir == null)
            {
                throw new ArgumentException("The file path is invalid.", nameof(fullPath));
            }

            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            // serialize the specified instance and write the resulting data to the specified file
            string jsonString = JsonSerializer.Serialize(model, this.JsonOptions);
            File.WriteAllText(fullPath, jsonString);
        }

        /// <summary>
        /// Asynchronously save a representation of the specified model to a JSON file.
        /// </summary>
        /// <typeparam name="TModel">The model type.</typeparam>
        /// <param name="fullPath">The absolete file path.</param>
        /// <param name="model">The model to save.</param>
        /// <exception cref="ArgumentException">The given path is empty or invalid.</exception>
        public async Task WriteJsonFileAsync<TModel>(string fullPath, TModel model)
            where TModel : class
        {
            // validate the specified path for correctness
            if (string.IsNullOrWhiteSpace(fullPath))
            {
                throw new ArgumentException("The file path is empty or invalid.", nameof(fullPath));
            }

            // create directory if needed
            string? dir = Path.GetDirectoryName(fullPath);
            if (dir == null)
            {
                throw new ArgumentException("The file path is invalid.", nameof(fullPath));
            }

            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            // serialize the specified insttance and write the resulting data to the specified file
            using (FileStream fs = File.Create(fullPath))
            {
                await JsonSerializer.SerializeAsync(fs, model, this.JsonOptions);
            }
        }
    }
}
