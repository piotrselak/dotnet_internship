﻿namespace contacts.Client.Domain;

public class ErrorResponse
{
    public string Type { get; set; }
    public string Title { get; set; }
    public int Status { get; set; }
    public string Detail { get; set; }
    public string TraceId { get; set; }
}