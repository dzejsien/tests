namespace GenerateRoutes
{
    public class TagRoutes
    {
        public const string RootApiRoutePrefix = "api/v1/tags";
        public const string BidsRoutePrefix = RootApiRoutePrefix + "/bids";
        public const string TagCollectionsPrefix = RootApiRoutePrefix + "/tag_collections";
        public const string TagCollectionsSpecifiedPrefix = TagCollectionsPrefix + "/specified";

        public const string BidIdParamUnquoted = "bidId";
        public const string BidIdParam = "{" + BidIdParamUnquoted + "}";

        public const string CollectionIdParamUnquoted = "collectionId";
        public const string CollectionIdParam = "{" + CollectionIdParamUnquoted + "}";

        public const string TagIdParamUnquoted = "tagId";
        public const string TagIdParam = "{" + TagIdParamUnquoted + "}";

        public const string AllPrefix = "/all";

        public const string BidCostGroupTagsPrefix = BidsRoutePrefix + "/" + BidIdParam + "/tags";

        public const string AllTagAssignmentsPrefix = BidCostGroupTagsPrefix + AllPrefix;

        public const string AllBidCostGroupTagsPrefix = BidCostGroupTagsPrefix + "/commodities" + AllPrefix;

        public const string BaselinesTagsPrefix = BidCostGroupTagsPrefix + "/baselines";
        public const string AllBaselinesTagsPrefix = BaselinesTagsPrefix + AllPrefix;

        public const string MetricsTagsPrefix = BidCostGroupTagsPrefix + "/metrics";
        public const string AllMetricsTagsPrefix = MetricsTagsPrefix + AllPrefix;

        public const string SumValSumCostTagsPrefix = BidCostGroupTagsPrefix + "/sumvalsumcost";
        public const string AllSumValSumCostTagsPrefix = SumValSumCostTagsPrefix + AllPrefix;

        public const string BidTagCollectionsPrefix = BidsRoutePrefix + "/" + BidIdParam + "/tag_collections";

        public const string BidAssignedTagCollectionsPrefix = BidsRoutePrefix + "/" + BidIdParam + "/assigned_tag_collections";
        public const string BidAllTagCollectionsAssignmentPrefix = BidTagCollectionsPrefix + "/assignment/all";
        public const string BidTagCollectionsSettingsPrefix = BidTagCollectionsPrefix + "/settings";
        public const string BidTagAssignmentSettingsPrefix = BidsRoutePrefix + "/" + BidIdParam + "/tag_assignment_settings";
        public const string BidTagCollectionsAssignmentPrefix = BidTagAssignmentSettingsPrefix + "/tag_collections";

        public const string TagCollectionPrefix = TagCollectionsPrefix + "/" + CollectionIdParam;
        public const string VisibilityPrefix = TagCollectionPrefix + "/visibility";
        public const string TagCollectionMaintenancePrefix = TagCollectionsPrefix + "/" + CollectionIdParam + "/tags";
        public const string TagCollectionMaintenanceTagPrefix = TagCollectionMaintenancePrefix + "/" + TagIdParam;

        public const string BidTagCollectionPrefix = BidTagCollectionsPrefix + "/" + CollectionIdParam;
        public const string BidTagCollectionMaintenancePrefix = BidTagCollectionsPrefix + "/" + CollectionIdParam + "/tags";
        public const string BidTagCollectionMaintenanceTagPrefix = BidTagCollectionMaintenancePrefix + "/" + TagIdParam;

        public const string TagCollectionTypesPrefix = RootApiRoutePrefix + "/tag_collection_types";

        public const string SystemStatuesPrefix = RootApiRoutePrefix + "/system_statuses";

        public const string BidTagCollectionsImportPrefix = BidTagCollectionsPrefix + "/import";
        public const string BidTagCollectionsImportValidatePrefix = BidTagCollectionsImportPrefix + "/validate";

        public const string StandardTagCollectionsImportPrefix = TagCollectionsPrefix + "/import";
        public const string StandardTagCollectionsImportValidatePrefix = StandardTagCollectionsImportPrefix + "/validate";

        public const string TemplatesRoutePrefix = RootApiRoutePrefix + "/templates";
        public const string TemplateTagCollectionsPrefix = TemplatesRoutePrefix + "/" + BidIdParam + "/tag_collections";
    }
}