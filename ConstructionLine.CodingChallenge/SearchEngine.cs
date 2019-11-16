using System.Collections.Generic;
using System.Linq;

namespace ConstructionLine.CodingChallenge
{
    public class SearchEngine
    {
        private readonly List<Shirt> _shirts;

        public SearchEngine(List<Shirt> shirts)
        {
            _shirts = shirts;

            // TODO: data preparation and initialisation of additional data structures to improve performance goes here.

        }


        public SearchResults Search(SearchOptions options)
        {
            #region Initiliaze Search Result
            SearchSizeCount shirtCountBySize = new SearchSizeCount();
            List<SizeCount> sizeCount = new List<SizeCount>();
            List<ColorCount> colorCount = new List<ColorCount>();

            #endregion

            // TODO: search logic goes here.
            var result = _shirts.Where(a => options.Sizes.Any(b => (b.Name == a.Size.Name)) && options.Colors.Any(c => c.Name == a.Color.Name)).ToList();
            
            #region this will get me the SizeCount data
            Size.All.ForEach(delegate (Size data)
            {
                var szCount = new SizeCount() { 
                    Size = data, 
                    Count = _shirts.Count(s => s.Size.Id == data.Id && (!options.Colors.Any() || options.Colors.Select(c => c.Id).Contains(s.Color.Id)))
                };
                sizeCount.Add(szCount);

            });

            #endregion

            #region this will get me the ColorCount Data
            Color.All.ForEach(delegate (Color data)
            {
                var colCount = new ColorCount() {
                    Color = data,
                    Count = _shirts.Count(s => s.Color.Id == data.Id && (!options.Sizes.Any() || options.Sizes.Select(c => c.Id).Contains(s.Size.Id)))
                };
                colorCount.Add(colCount);
            });
            #endregion


            return new SearchResults
            {
                Shirts = result,
                SizeCounts = sizeCount,
                ColorCounts = colorCount
            };
        }
    }
}