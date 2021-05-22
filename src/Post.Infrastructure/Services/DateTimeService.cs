using System;

using Post.Application.Common.Interfaces;

namespace Post.Infrastructure.Services {
    public class DateTimeService : IDateTime {
        public DateTime Now => DateTime.Now;
    }
}