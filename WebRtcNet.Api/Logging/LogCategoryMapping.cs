using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace WebRtcNet.Logging;

/// <summary>
/// Maps WebRTC log tags to categories and EventId base values.
/// Loads mapping from JSON resource at startup.
/// </summary>
internal class LogCategoryMapping
{
	private readonly List<CategoryMapping> mappings_ = [];

	/// <summary>
	/// Represents a single tag-to-category mapping entry.
	/// </summary>
	private class CategoryMapping
	{
		[JsonPropertyName("tagPattern")]
		public string TagPattern { get; set; } = string.Empty;

		[JsonPropertyName("category")]
		public string Category { get; set; } = string.Empty;

		[JsonPropertyName("eventIdBase")]
		public int EventIdBase { get; set; }

		public Regex CompiledPattern { get; set; } = null!;
	}

	/// <summary>
	/// Initializes the mapping from embedded JSON resource.
	/// </summary>
	public static LogCategoryMapping LoadFromResource()
	{
		var mapping = new LogCategoryMapping();

		try
		{
			// Load JSON from embedded resource
			var assembly = typeof(LogCategoryMapping).Assembly;
			var resourceName = $"{typeof(LogCategoryMapping).Namespace}.Resources.LogCategoryMapping.json";

			using var stream = assembly.GetManifestResourceStream(resourceName);
			if (stream == null)
				throw new InvalidOperationException($"Resource '{resourceName}' not found.");

			using var reader = new System.IO.StreamReader(stream);
			var json = reader.ReadToEnd();

			var root = JsonDocument.Parse(json);
			var element = root.RootElement.GetProperty("logCategoryMappings");

			foreach (var item in element.EnumerateArray())
			{
				var tagPattern = item.GetProperty("tagPattern").GetString() ?? string.Empty;
				var category = item.GetProperty("category").GetString() ?? string.Empty;
				var eventIdBase = item.GetProperty("eventIdBase").GetInt32();

				mapping.mappings_.Add(new CategoryMapping
				{
					TagPattern = tagPattern,
					Category = category,
					EventIdBase = eventIdBase,
					CompiledPattern = new Regex(tagPattern, RegexOptions.Compiled)
				});
			}
		}
		catch (Exception ex)
		{
			throw new InvalidOperationException("Failed to load log category mapping resource.", ex);
		}

		return mapping;
	}

	/// <summary>
	/// Resolves a WebRTC tag to category and EventId base.
	/// Returns ("WebRTC.Other", 1600) if no match found.
	/// </summary>
	public (string Category, int EventIdBase) ResolveTagToCategory(string tag)
	{
		Contract.Requires<ArgumentException>(!string.IsNullOrEmpty(tag), nameof(tag));

		// Remove leading/trailing parentheses if present (tags come as "(tag_name)")
		var cleanTag = tag.Trim('(', ')');

		foreach (var mapping in mappings_)
		{
			if (mapping.CompiledPattern.IsMatch(cleanTag))
				return (mapping.Category, mapping.EventIdBase);
		}

		// Default fallback
		return ("WebRTC.Other", 1600);
	}
}
