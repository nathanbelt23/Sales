﻿
namespace Sales.Common.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Product
    {
        [Key]
        public int ProductId { get; set; }

        [Required]
        [StringLength(50)]
        public string Description { get; set; }



        [DataType(DataType.MultilineText)]
        public string Remarks { get; set; }

        [Display(Name = "Image")]
        public string ImagePath { get; set; }

        [DisplayFormat(DataFormatString = "{0:C2}", ApplyFormatInEditMode = false)]
        public decimal Price { get; set; }

        [Display(Name = "Is Variable")]
        public bool IsVariable { get; set; }

        [Display(Name = "Publish On")]
        [DataType(DataType.Date)]
        public DateTime PublishOn { get; set; }

        [NotMapped]
        public byte[] ImageArray { get; set; }


        public  string ImageFullPath 
            {
            get {
                if (string.IsNullOrEmpty(ImagePath))
                {
                    return "NoProduct.png";
                }
                return $"http://192.168.0.11/SalesBACKEND{ImagePath.Replace("~","")}";
            }
            }


        public override string ToString()
        {
            return $"{ProductId.ToString()},{"-"+Description }";
        }
    }
}
