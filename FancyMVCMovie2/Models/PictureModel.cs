using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace FancyMVCMovie2.Models
{
    public class PictureModel
    {
        [Key]
        public int Id { get; set; }
        public byte[] ImageFull { get; set; }
        public byte[] ImageThumbnail { get; set; }

        [Required]
        public int MovieId { get; set; }
        [ForeignKey("MovieId")]
        public virtual Movie Movie { get; set; }

        [NotMapped]
        public Guid TempGuid { get; set; }

        public PictureModel Clone()
        {
            return new PictureModel()
            {
                Id = this.Id,
                ImageFull = (Byte[])this.ImageFull.Clone(),
                ImageThumbnail = (Byte[])this.ImageThumbnail.Clone(),
                TempGuid = this.TempGuid
            };
        }
    }
}