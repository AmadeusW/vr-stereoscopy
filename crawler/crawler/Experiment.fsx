#r "FSharp.Data.dll"
open FSharp.Data

type GoogleSearchResultPage = HtmlProvider<"https://www.reddit.com/r/CrossView/top/">
let firstPage = GoogleSearchResultPage.Load("https://www.reddit.com/r/CrossView/top/")
let secondPage = GoogleSearchResultPage.Load("https://www.reddit.com/r/CrossView/top/?count=25&after=t3_5cao1s")

firstPage.Html.Descendants()
|> Seq.filter (fun n -> n.HasName("a") && n.HasClass("title"))
|> Seq.iter (fun n -> printfn "%s" (n.InnerText()))