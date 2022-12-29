namespace FlowerShopManagement.Core.Entities;

public class Material
{
	public string _id { get; set; } 
	public string _name { get; set; } 
	public string _maintainment { get; set; }

	public Material(string id = "1", string name = "Unknown", string maintainment = "blank") {
		_id= id;
		_name= name;
		_maintainment= maintainment;
	}

}

