using Raven.Abstractions;
using Raven.Database.Linq;
using System.Linq;
using System.Collections.Generic;
using System.Collections;
using System;
using Raven.Database.Linq.PrivateExtensions;
using Lucene.Net.Documents;
using System.Globalization;
using System.Text.RegularExpressions;
using Raven.Database.Indexing;


public class Index_LatestVersionOfConfigData : Raven.Database.Linq.AbstractViewGenerator
{
	public Index_LatestVersionOfConfigData()
	{
		this.ViewText = @"docs.NameValueModels.Select(model => new {
    Name = model.Name,
    Version = model.Version
})
results.GroupBy(result => new {
    Name = result.Name,
    Version = result.Version
}).Select(g => new {
    Name = g.Key.Name,
    Version = DynamicEnumerable.Max(g.Select(x => x.Version))
})";
		this.ForEntityNames.Add("NameValueModels");
		this.AddMapDefinition(docs => docs.Where(__document => string.Equals(__document["@metadata"]["Raven-Entity-Name"], "NameValueModels", System.StringComparison.InvariantCultureIgnoreCase)).Select((Func<dynamic, dynamic>)(model => new {
			Name = model.Name,
			Version = model.Version,
			__document_id = model.__document_id
		})));
		this.ReduceDefinition = results => results.GroupBy((Func<dynamic, dynamic>)(result => new {
			Name = result.Name,
			Version = result.Version
		})).Select((Func<IGrouping<dynamic,dynamic>, dynamic>)(g => new {
			Name = g.Key.Name,
			Version = DynamicEnumerable.Max(g.Select((Func<dynamic, dynamic>)(x => x.Version)))
		}));
		this.GroupByExtraction = result => new {
			Name = result.Name,
			Version = result.Version
		};
		this.AddField("Name");
		this.AddField("Version");
	}
}
