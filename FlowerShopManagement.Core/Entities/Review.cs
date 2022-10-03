namespace FlowerShopManagement.Core.Entities
{
    public class Review : BaseEntity
    {
        protected Guid _productID { get; set; }
        protected Guid _authorID { get; set; }
        protected string _authorName { get; set; }
        protected string _authorAvatar { get; set; }
        protected string? _title { get; set; }
        protected string _body { get; set; }
        protected int _likes { get; set; }
        protected int _dislikes { get; set; }
        protected DateTime _createdDate { get; set; }
        protected DateTime _lastModified { get; set; }
    }
}
