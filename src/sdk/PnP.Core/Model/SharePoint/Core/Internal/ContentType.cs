﻿using PnP.Core.Services;
using System.Dynamic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace PnP.Core.Model.SharePoint
{
    [SharePointType("SP.ContentType", Target = typeof(Web),
        Uri = "_api/Web/ContentTypes('{Id}')", Get = "_api/web/ContentTypes", LinqGet = "_api/web/ContentTypes")]
    [SharePointType("SP.ContentType", Target = typeof(List), Uri = "_api/Web/Lists(guid'{Parent.Id}')/ContentTypes('{Id}')",
        Get = "_api/Web/Lists(guid'{Parent.Id}')/ContentTypes", LinqGet = "_api/Web/Lists(guid'{Parent.Id}')/ContentTypes")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2243:Attribute string literals should parse correctly", Justification = "<Pending>")]
    internal partial class ContentType : BaseDataModel<IContentType>, IContentType
    {
        #region Construction
        public ContentType()
        {
            PostMappingHandler = (json) =>
            {
                // In the case of Add operation, the CSOM api is used and JSON mapping to model is not possible
                // So here, populate the Rest Id metadata field to enable actions upon 
                // this content type without having to read it again from the server
                AddMetadata(PnPConstants.MetaDataRestId, StringId);
                AddMetadata(PnPConstants.MetaDataType, "SP.ContentType");
                Requested = true;
            };

            // Handler to construct the Add request for this content type
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
            AddApiCallHandler = async (keyValuePairs) =>
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
            {
                // Content type creation by specifying the parent id is not possible using SharePoint REST
                //https://github.com/pnp/pnpjs/issues/457
                //https://github.com/SharePoint/sp-dev-docs/issues/3276
                //https://stackoverflow.com/questions/55529315/how-to-create-site-content-type-with-id-using-rest-api

                // Given this method can apply on both Web.ContentTypes as List.ContentTypes we're getting the entity info which will 
                // automatically provide the correct 'parent'
                var entity = EntityManager.GetClassInfo(GetType(), this);

                // Adding new content types on a list is not something we should allow
                if (entity.Target == typeof(List))
                {
                    throw new ClientException(ErrorType.Unsupported,
                        PnPCoreResources.Exception_Unsupported_AddingContentTypeToList);
                }

                // Fallback to CSOM call
                string actualDescription = !string.IsNullOrEmpty(Description)
                    ? $@"<Property Name=""Description"" Type=""String"">{CsomHelper.XmlString(Description)}</Property>"
                    : $@"<Property Name=""Description"" Type=""Null"" />";
                string actualGroup = !string.IsNullOrEmpty(Group)
                    ? $@"<Property Name=""Group"" Type=""String"">{CsomHelper.XmlString(Group)}</Property>"
                    : @"<Property Name=""Group"" Type=""Null"" />";

                string xml = CsomHelper.ContentTypeCreate.Replace(CsomHelper.ContentTypeActualDescription, actualDescription)
                                                         .Replace(CsomHelper.ContentTypeActualGroup, actualGroup)
                                                         .Replace(CsomHelper.ContentTypeStringId, CsomHelper.XmlString(StringId))
                                                         .Replace(CsomHelper.ContentTypeName, CsomHelper.XmlString(Name));

                return new ApiCall(xml);
            };
        }
        #endregion

        #region Properties
        public string StringId { get => GetValue<string>(); set => SetValue(value); }

        [SharePointProperty("Id", JsonPath = "StringValue")]
        public string Id { get => GetValue<string>(); set => SetValue(value); }

        public string ClientFormCustomFormatter { get => GetValue<string>(); set => SetValue(value); }

        public string Description { get => GetValue<string>(); set => SetValue(value); }

        public string DisplayFormTemplateName { get => GetValue<string>(); set => SetValue(value); }

        public string DisplayFormUrl { get => GetValue<string>(); set => SetValue(value); }

        public string DocumentTemplate { get => GetValue<string>(); set => SetValue(value); }

        public string DocumentTemplateUrl { get => GetValue<string>(); set => SetValue(value); }

        public string EditFormTemplateName { get => GetValue<string>(); set => SetValue(value); }

        public string EditFormUrl { get => GetValue<string>(); set => SetValue(value); }

        public string Group { get => GetValue<string>(); set => SetValue(value); }

        public bool Hidden { get => GetValue<bool>(); set => SetValue(value); }

        public string JSLink { get => GetValue<string>(); set => SetValue(value); }

        public string MobileDisplayFormUrl { get => GetValue<string>(); set => SetValue(value); }

        public string MobileEditFormUrl { get => GetValue<string>(); set => SetValue(value); }

        public string MobileNewFormUrl { get => GetValue<string>(); set => SetValue(value); }

        public string Name { get => GetValue<string>(); set => SetValue(value); }

        public string NewFormTemplateName { get => GetValue<string>(); set => SetValue(value); }

        public string NewFormUrl { get => GetValue<string>(); set => SetValue(value); }

        public bool ReadOnly { get => GetValue<bool>(); set => SetValue(value); }

        public string SchemaXml { get => GetValue<string>(); set => SetValue(value); }

        public string SchemaXmlWithResourceTokens { get => GetValue<string>(); set => SetValue(value); }

        public string Scope { get => GetValue<string>(); set => SetValue(value); }

        public bool Sealed { get => GetValue<bool>(); set => SetValue(value); }

        public IFieldLinkCollection FieldLinks { get => GetModelCollectionValue<IFieldLinkCollection>(); }

        [KeyProperty(nameof(StringId))]
        public override object Key { get => StringId; set => StringId = value.ToString(); }
        #endregion

        #region Extension methods
        #region AddAvailableContentType
        private ApiCall AddAvailableContentTypeApiCall(string id)
        {
            dynamic body = new ExpandoObject();
            body.contentTypeId = id;

            var bodyContent = JsonSerializer.Serialize(body, typeof(ExpandoObject), new JsonSerializerOptions { WriteIndented = false });

            // Given this method can apply on both Web.ContentTypes as List.ContentTypes we're getting the entity info which will 
            // automatically provide the correct 'parent'
            var entity = EntityManager.GetClassInfo(GetType(), this);

            return new ApiCall($"{entity.SharePointGet}/AddAvailableContentType", ApiType.SPORest, bodyContent);
        }

        internal async Task<IContentType> AddAvailableContentTypeBatchAsync(Batch batch, string id)
        {
            var apiCall = AddAvailableContentTypeApiCall(id);
            await RequestBatchAsync(batch, apiCall, HttpMethod.Post).ConfigureAwait(false);
            return this;
        }

        internal async Task<IContentType> AddAvailableContentTypeAsync(string id)
        {
            var apiCall = AddAvailableContentTypeApiCall(id);
            await RequestAsync(apiCall, HttpMethod.Post).ConfigureAwait(false);
            return this;
        }
        #endregion
        #endregion
    }
}
