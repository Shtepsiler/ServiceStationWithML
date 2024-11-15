﻿namespace GeneralBusMessages.Message
{
    public class PartImage
    {
        public Guid Id { get; set; }
        public DateTime Timestamp { get; set; }
        public string? SourceContentType { get; set; }
        public string? HashFileContent { get; set; }
        public int? Size { get; set; }
        public string? Path { get; set; }
        public string? Title { get; set; }
        public Guid PartId { get; set; }
    }
}
