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


public class Index_Auto_2fNameValueModels_2fByGroupNameAndNameAndVersion : Raven.Database.Linq.AbstractViewGenerator
{
	public Index_Auto_2fNameValueModels_2fByGroupNameAndNameAndVersion()
	{
		this.ViewText = @"from doc in docs.NameValueModels
select new { Version = doc.Version, Name = doc.Name, GroupName = doc.GroupName }";
		this.ForEntityNames.Add("NameValueModels");
		this.AddMapDefinition(docs => 
			from doc in docs
			where string.Equals(doc["@metadata"]["Raven-Entity-Name"], "NameValueModels", System.StringComparison.InvariantCultureIgnoreCase)
			select new {
				Version = doc.Version,
				Name = doc.Name,
				GroupName = doc.GroupName,
				__document_id = doc.__document_id
			});
		this.AddField("Version");
		this.AddField("Name");
		this.AddField("GroupName");
		this.AddField("__document_id");
		this.AddQueryParameterForMap("Version");
		this.AddQueryParameterForMap("Name");
		this.AddQueryParameterForMap("GroupName");
		this.AddQueryParameterForMap("__document_id");
		this.AddQueryParameterForReduce("Version");
		this.AddQueryParameterForReduce("Name");
		this.AddQueryParameterForReduce("GroupName");
		this.AddQueryParameterForReduce("__document_id");
	}
}
