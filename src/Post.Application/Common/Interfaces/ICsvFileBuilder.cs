using System.Collections.Generic;

using Post.Application.Zones.Queries.ExportDeliveries;

namespace Post.Application.Common.Interfaces {
    public interface ICsvFileBuilder {
        byte[] BuildDeliveriesFile(IEnumerable<DeliveryRecord> records);
    }
}