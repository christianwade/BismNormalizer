using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AnalysisServices.Tabular;
using Tom=Microsoft.AnalysisServices.Tabular;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace BismNormalizer.TabularCompare.TabularMetadata
{
    /// <summary>
    /// Represents a tabular object for comparison. This class handles JSON serialization.
    /// </summary>
    public class TabularObject
    {
        private string _objectDefinition;
        private string _name;

        /// <summary>
        /// Initializes a new instance of the TabularObject class.
        /// </summary>
        /// <param name="namedMetaDataObject">The Tabular Object Model supertype of the class being abstracted.</param>
        public TabularObject(NamedMetadataObject namedMetaDataObject)
        {
            _name = namedMetaDataObject.Name;
            
            //Serialize json
            SerializeOptions options = new SerializeOptions();
            options.IgnoreInferredProperties = true;
            options.IgnoreInferredObjects = true;
            options.IgnoreTimestamps = true;
            options.SplitMultilineStrings = true;
            _objectDefinition = Tom.JsonSerializer.SerializeObject(namedMetaDataObject, options);

            //Remove annotations
            JToken token = JToken.Parse(_objectDefinition);
            RemoveAnnotationsFromObjectDefinition(token);
            _objectDefinition = token.ToString(Formatting.Indented);

            //Order table columns
            if (namedMetaDataObject is Tom.Table)
            {
                _objectDefinition = SortTableColumns(_objectDefinition);
            }
        }

        private string SortTableColumns(string json)
        {
            JObject jObj = (JObject)JsonConvert.DeserializeObject(json);

            foreach (var prop in jObj.Properties())
            {
                if (prop.Value.Type == JTokenType.Array && prop.Name == "columns")
                {
                    var vals = prop.Values()
                        .OfType<JObject>()
                        .OrderBy(x => x.Property("name").Value.ToString())
                        .ToList();
                    prop.Value = JContainer.FromObject(vals);
                }
            }

            return jObj.ToString(Formatting.Indented);
        }

        private void RemoveAnnotationsFromObjectDefinition(JToken token)
        {
            //child object annotations
            List<JToken> removeList = new List<JToken>();
            foreach (JToken childToken in token.Children())
            {
                JProperty property = childToken as JProperty;
                if (property != null && property.Name == "annotations")
                {
                    removeList.Add(childToken);
                }
                RemoveAnnotationsFromObjectDefinition(childToken);
            }
            foreach (JToken tokenToRemove in removeList)
            {
                tokenToRemove.Remove();
            }
        }

        /// <summary>
        /// Explicitly remove a JSON property from definition. An example of this is removing parititions from table definitions.
        /// </summary>
        /// <param name="propertyToRemove"></param>
        public void RemovePropertyFromObjectDefinition(string propertyToRemove)
        {
            JObject jobj = JObject.Parse(_objectDefinition);
            jobj.Remove(propertyToRemove);
            _objectDefinition = jobj.ToString(Formatting.Indented);
        }

        /// <summary>
        /// The serialized JSON definition of the tabular object.
        /// </summary>
        public string ObjectDefinition => _objectDefinition;

        /// <summary>
        /// The name of the tabular object. Gets overriden by Relationship to show friendly name.
        /// </summary>
        public virtual string Name => _name;

        /// <summary>
        /// The internal name of the tabular object. Gets overriden by Relationship to store the true name from TOM (GUID form).
        /// </summary>
        public virtual string InternalName => _name; 

    }
}

