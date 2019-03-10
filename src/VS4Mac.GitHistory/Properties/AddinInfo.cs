using System;
using System.Runtime.InteropServices;
using Mono.Addins;
using Mono.Addins.Description;

[assembly: Addin(
	"GitHistory",
	Namespace = "MonoDevelop",
	Version = "0.1"
)]

[assembly: AddinName("Git History")]
[assembly: AddinCategory("IDE extensions")]
[assembly: AddinDescription("Quickly browse the history of a file from any git repository directly from VS4Mac.")]
[assembly: AddinAuthor("Javier Suárez")]
[assembly: AddinUrl("https://github.com/jsuarezruiz/VS4Mac-GitHistory")]

[assembly: CLSCompliant(false)]
[assembly: ComVisible(false)]