﻿namespace Movieverse.Application.Payments.PayPal.Models;

public sealed class Link
{
	public string href { get; set; }
	public string rel { get; set; }
	public string method { get; set; }
}