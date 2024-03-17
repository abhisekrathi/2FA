using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace _2FA.Models
{
	[Table("Code")]
	public class Code
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public long CodeId {  get; set; }
		public string Mobile { get; set; }
		[ForeignKey("Mobile")]
		public User User { get; set; }
		public string CodeValue { get; set; }
		public DateTime GenerationTime { get; set; }
		public DateTime? UtilizationTime { get; set; }
	}
}