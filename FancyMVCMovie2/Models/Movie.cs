﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace FancyMVCMovie2.Models
{
    public class Movie
    {
        public Movie()
        {
            this.Pictures = new List<PictureModel>();
        }

        [Key]
        public int? Id { get; set; }
        
        public string UserId { get; set; }

        [StringLength(60, MinimumLength = 3)]
        [Display(Name = "Title")]
        [Required]
        public string MovieTitle { get; set; }

        [Display(Name = "Release Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:d MMMM, yyyy}", ApplyFormatInEditMode = true)]
        public DateTime ReleaseDateTime { get; set; }

        [Required]
        public int GenreId { get; set; }

        [ForeignKey("GenreId")]
        [Display(Name = "Genre")]
        public virtual Genre Genre { get; set; }

        [Range(1, 100)]
        [DataType(DataType.Currency)]
        [Display(Name = "Price")]
        public decimal Price { get; set; }

        [InverseProperty("Movie")]
        public virtual List<PictureModel> Pictures { get; set; }
    }
}