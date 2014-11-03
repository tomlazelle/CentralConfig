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


public class Index_Auto_2fNameValueModels_2fByGroupNameAndName : Raven.Database.Linq.AbstractViewGenerator
{
	public Index_Auto_2fNameValueModels_2fByGroupNameAndName()
	{
		this.ViewText = @"from doc in docs.NameValueModels
select new { GroupName = doc.GroupName, Name = doc.Name }";
		this.ForEntityNames.Add("NameValueModels");
		this.AddMapDefinition(docs => 
			from doc in docs
			where string.Equals(doc["@metadata"]["Raven-Entity-Name"], "NameValueModels", System.StringComparison.InvariantCultureIgnoreCase)
			select new {
				GroupName = doc.GroupName,
				Name = doc.Name,
				__document_id = doc.__document_id
			});
		this.AddField("GroupName");
		this.AddField("Name");
		this.AddField("__document_id");
		this.AddQueryParameterForMap("GroupName");
		this.AddQueryParameterForMap("Name");
		this.AddQueryParameterForMap("__document_id");
		this.AddQueryParameterForReduce("GroupName");
		this.AddQueryParameterForReduce("Name");
		this.AddQueryParameterForReduce("__document_id");
	}
}
