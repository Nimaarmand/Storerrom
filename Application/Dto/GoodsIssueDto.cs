using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Dto
{
    public class GoodsIssueDto
    {
        public int IssueId { get; set; }
        public string ProductName { get; set; }
        public string WarehouseName { get; set; }
        public string CustomerName { get; set; }      
        public string UserName { get; set; }
        public decimal Quantity { get; set; }
        public string Unit { get; set; }
        public decimal? UnitSellingPrice { get; set; }
        public decimal TotalPrice => (UnitSellingPrice ?? 0) * Quantity;
        public string InvoiceNumber { get; set; }
        public DateTime InvoiceDate { get; set; }
        public DateTime IssueDate { get; set; }
        public string StatusText { get; set; }          // وضعیت (ثبت، تایید، لغو)
        public string BatchNumber { get; set; }
        public string Description { get; set; }
    }
}
