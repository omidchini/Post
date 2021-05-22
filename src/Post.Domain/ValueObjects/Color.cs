using System.Collections.Generic;
using System.Linq;

using Post.Domain.Common;
using Post.Domain.Exceptions;

namespace Post.Domain.ValueObjects {
    public class Color : ValueObject {
        public static Color Blue => new Color("#6666FF");

        public static Color Green => new Color("#CCFF99 ");

        public static Color Grey => new Color("#999999");

        public static Color Orange => new Color("#FFC300");

        public static Color Purple => new Color("#9966CC");

        public static Color Red => new Color("#FF5733");

        public static Color White => new Color("#FFFFFF");

        public static Color Yellow => new Color("#FFFF66");

        protected static IEnumerable<Color> SupportedColors {
            get {
                yield return White;
                yield return Red;
                yield return Orange;
                yield return Yellow;
                yield return Green;
                yield return Blue;
                yield return Purple;
                yield return Grey;
            }
        }

        public static Color From(string code) {
            var color = new Color { Code = code };

            if (!SupportedColors.Contains(color)) {
                throw new UnsupportedColorException(code);
            }

            return color;
        }

        private Color() { }

        private Color(string code) {
            Code = code;
        }

        public string Code { get; private set; }

        public static implicit operator string(Color color) {
            return color.ToString();
        }

        public static explicit operator Color(string code) {
            return From(code);
        }

        public override string ToString() {
            return Code;
        }

        protected override IEnumerable<object> GetEqualityComponents() {
            yield return Code;
        }
    }
}