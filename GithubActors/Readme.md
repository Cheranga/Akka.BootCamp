> When using the GITHUB Authentication token with .NET framework 4.5, this change needs to be done

* Had to add the below code in the `Program.cs` according to the fix
`ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;`

https://github.com/octokit/octokit.net/issues/1756
