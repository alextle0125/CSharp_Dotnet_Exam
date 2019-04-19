using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
 
namespace ActivityCenter.Models
{
	public class User
	{
	    [Key]
	    public int UserId {get;set;}
	   	[Required]
	   	[MinLength(2, ErrorMessage="Name must be 2 characters or longer!")]
	    public string Name {get;set;}
	    [EmailAddress]
	    [Required]
	    public string Email {get;set;}
	    [DataType(DataType.Password)]
	    [Required]
	    [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d)(?=.*[@$!%*#?&])[A-Za-z\d@$!%*#?&]{8,}$", ErrorMessage="Password must be a minimum of characters and contain at least 1 letter, 1 number, and 1 special character.")]
	    [MinLength(8, ErrorMessage="Password  or longer!")]
	    public string Password {get;set;}
	    public DateTime CreatedAt {get;set;} = DateTime.Now;
	    public DateTime UpdatedAt {get;set;} = DateTime.Now;
	    public List<ACActivity> ACActivities {get;set;}
	    // Will not be mapped to your users table!
	    [NotMapped]
	    [Compare("Password", ErrorMessage="Passwords must match")]
	    [DataType(DataType.Password)]
	    public string PasswordConfirm {get;set;}
	    public string Display()
	    {
	    	return String.Format("<Entity: User>Email: {2}, Password: {3}, CreatedAt: {4}, UpdatedAt: {5}", Name, Email, Password, CreatedAt, UpdatedAt);
	    }
	} 
}