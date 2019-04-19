using System;
using System.ComponentModel.DataAnnotations;
 
namespace ActivityCenter.Models
{
	public class Join
	{
	    [Key]
	    public int JoinId {get;set;}
	    public int UserId {get;set;}
	    public int ACActivityId {get;set;}
	    public User User {get; set;}
	    public ACActivity ACActivity {get;set;}
	    public DateTime CreatedAt {get;set;} = DateTime.Now;
	    public DateTime UpdatedAt {get;set;} = DateTime.Now;
	} 
}