using System.Globalization;

using CsvHelper.Configuration;

using Post.Application.Zones.Queries.ExportDeliveries;

namespace Post.Infrastructure.Files.Maps {
    public class DeliveryRecordMap : ClassMap<DeliveryRecord> {
        public DeliveryRecordMap() {
            AutoMap(CultureInfo.InvariantCulture);
            Map(m => m.Done).ConvertUsing(c => c.Done ? "Yes" : "No");
        }
    }
}