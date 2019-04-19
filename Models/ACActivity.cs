using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
 
namespace ActivityCenter.Models
{
	public class ACActivity
	{
	    [Key]
	    public int ACActivityId {get;set;}
	    [Required]
	    public string Title {get;set;}
	    [Required]
	    public DateTime Date {get;set;}
	    public string Duration {get;set;}
	    [Required]
	    public string Description {get;set;}
	    public int UserId {get;set;}
	   	public User User {get;set;}
	    public List<Join> Participants {get;set;} = new List<Join>();
	    public DateTime CreatedAt {get;set;} = DateTime.Now;
	    public DateTime UpdatedAt {get;set;} = DateTime.Now;
	} 
}