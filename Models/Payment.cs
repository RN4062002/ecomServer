using System;
using System.Collections.Generic;

namespace ecomServer.Models;

public partial class Payment
{
    public int PaymentId { get; set; }

    public int OrderId { get; set; }

    public string? PaymentMode { get; set; }

    public string? PaymentStatus { get; set; }

    public string? TransactionId { get; set; }

    public decimal? PaidAmount { get; set; }

    public DateTime? PaidDate { get; set; }

    public virtual Order Order { get; set; } = null!;
}
