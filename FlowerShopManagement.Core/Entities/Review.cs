using FlowerShopManagement.Core.Common;
using System.ComponentModel.DataAnnotations;

namespace FlowerShopManagement.Core.Entities;

public class Review : BaseEntity
{
    [Required]
    public string _productID { get; set; }
    [Required]
    public string _authorID { get; set; }
    public string _authorName { get; set; }
    public string _authorAvatar { get; set; }
    public string? _title { get; set; }
    [Required]
    public string _body { get; set; }
    public int _likes { get; set; }
    public int _dislikes { get; set; }
    public DateTime _createdDate { get; set; }
    public DateTime _lastModified { get; set; }

    public Review(
        string productID, string authorID, string authorName, string authorAvatar, 
        string? title, string body, 
        DateTime createdDate, DateTime lastModified)
    {
        _productID = productID;
        _authorID = authorID;
        _authorName = authorName;
        _authorAvatar = authorAvatar;
        _title = title;
        _body = body;
        _likes = 0;
        _dislikes = 0;
        _createdDate = createdDate;
        _lastModified = lastModified;
    }
}
