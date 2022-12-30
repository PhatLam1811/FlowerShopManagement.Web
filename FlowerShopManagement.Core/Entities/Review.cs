using System.ComponentModel.DataAnnotations;

namespace FlowerShopManagement.Core.Entities;

public class Review
{
    public string? _id { get; private set; }

    [Required]
    public string productID { get; set; }
    [Required]
    public string authorID { get; set; }
    public string authorName { get; set; }
    public string authorAvatar { get; set; }
    public string? title { get; set; }
    [Required]
    public string body { get; set; }
    public int likes { get; set; }
    public int dislikes { get; set; }
    public DateTime createdDate { get; set; }
    public DateTime lastModified { get; set; }

    public Review(
        string productID, string authorID, string authorName, string authorAvatar,
        string? title, string body,
        DateTime createdDate, DateTime lastModified)
    {
        this.productID = productID;
        this.authorID = authorID;
        this.authorName = authorName;
        this.authorAvatar = authorAvatar;
        this.title = title;
        this.body = body;
        this.likes = 0;
        this.dislikes = 0;
        this.createdDate = createdDate;
        this.lastModified = lastModified;
    }
}
