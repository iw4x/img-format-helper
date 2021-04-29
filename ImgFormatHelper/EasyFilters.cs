using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IWImgViewer
{
    class EasyFilters
    {
        struct Filter
        {
            public string extension;
            public string description;
        }

        List<Filter> filters = new List<Filter>();

        public EasyFilters(params (string description, string ext)[] textFilters)
        {
            Add(textFilters);
        }

        public void Add(params (string description, string ext)[] textFilters)
        {
            for (int i = 0; i < textFilters.Length; i++)
            {
                filters.Add(new Filter() { extension = textFilters[i].ext, description = textFilters[i].description });
            }
        }

        public string GetFilterExtensionByOneBasedIndex(int index)
        {
            return filters[index-1].extension;
        }

        public string GetFilterDescriptionByOneBasedIndex(int index)
        {
            return filters[index - 1].description;
        }

        public override string ToString()
        {
            return string.Join("|", filters.Select(o => $"{o.description} (*.{o.extension})|*.{o.extension}"));
        }
    }
}
