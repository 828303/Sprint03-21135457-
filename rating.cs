using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace GateBoys.Models
{
    public class rating
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }

        [Display(Name ="Product ID")]
        public int productId { get; set; }

        [Display(Name = "User Rate")]
        public int userRate { get; set; }

        [Display(Name = "Comment")]
        [DataType(DataType.MultilineText)]
        public string comment { get; set; }

        [Display(Name = "Attatchment")]
        public byte[] ratePic { get; set; }

        [Display(Name = "Rated By")]
        public string userEmail { get; set; }

        [Display(Name ="Rating Date")]
        public DateTime rateDate { get; set; }
        public bool replied { get; set; }

    }
    public class ratingReply
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }

        [Display(Name = "Replying To")]
        public int ratingId { get; set; }

        [Display(Name = "Comment")]
        [DataType(DataType.MultilineText)]
        public string comment { get; set; }

        [Display(Name = "Profile Picture")]
        public byte[] replyPic { get; set; }

        [Display(Name = "Rated By")]
        public string userEmail { get; set; }

        [Display(Name = "Reply Date")]
        public DateTime replyDate { get; set; }

    }
}