using System;
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
            List<Shirt> result = null;
            SearchSizeCount shirtCountBySize = new SearchSizeCount();
            List<SizeCount> sizeCount = new List<SizeCount>();
            List<ColorCount> colorCount = new List<ColorCount>();

            #endregion

            #region Search criteria
            // TODO: search logic goes here.
            //Both search criteria 
            Func<List<Shirt>, SearchOptions, List<Shirt>> bothFilter = (a, b) => a.Where(x => b.Colors.Contains(x.Color) && b.Sizes.Contains(x.Size)).ToList();

            //for Size only
            Func<List<Shirt>, List<Size>, List<Shirt>> sizeOnly = (a, b) => a.Where(x => b.Contains(x.Size)).ToList();

            //for Color only
            Func<List<Shirt>, List<Color>, List<Shirt>> colorOnly = (a, b) => a.Where(x => b.Contains(x.Color)).ToList();

            if(options.Sizes.Count > 0 && options.Colors.Count > 0)
            {
                result = bothFilter(_shirts, options);
            }
            else if(options.Sizes.Count == 0 && options.Colors.Count > 0)
            {
                result = colorOnly(_shirts, options.Colors);
            }
            else if(options.Sizes.Count > 0 && options.Colors.Count == 0)
            {
                result = sizeOnly(_shirts, options.Sizes);
            }

            #endregion

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