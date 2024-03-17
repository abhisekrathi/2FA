using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace _2FA.Models
{
	[Table("User")]
	public class User
	{
		[Key]
		public string Mobile { get; set; }
		public string Password { get; set; }
		public virtual ICollection<Code> Codes { get; set; }
	}
}