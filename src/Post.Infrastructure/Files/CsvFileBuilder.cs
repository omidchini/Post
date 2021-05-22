using System.Collections.Generic;
using System.Globalization;
using System.IO;

using CsvHelper;

using Post.Application.Common.Interfaces;
using Post.Application.Zones.Queries.ExportDeliveries;
using Post.Infrastructure.Files.Maps;

namespace Post.Infrastructure.Files {
    public class CsvFileBuilder : ICsvFileBuilder {
        public byte[] BuildDeliveriesFile(IEnumerable<DeliveryRecord> records) {
            using var memoryStream = new MemoryStream();
            using (var streamWriter = new StreamWriter(memoryStream)) {
                using var csvWriter = new CsvWriter(streamWriter, CultureInfo.InvariantCulture);

                csvWriter.Configuration.RegisterClassMap<DeliveryRecordMap>();
                csvWriter.WriteRecords(records);
            }

            return memoryStream.ToArray();
        }
    }
}