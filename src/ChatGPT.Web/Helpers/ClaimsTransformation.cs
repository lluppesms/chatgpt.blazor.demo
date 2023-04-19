//-----------------------------------------------------------------------
// <copyright file="MyClaimsTransformation.cs" company="Luppes Consulting, Inc.">
// Copyright 2023, Luppes Consulting, Inc. All rights reserved.
// </copyright>
// <summary>
// Claims Transformation Module
// </summary>
//-----------------------------------------------------------------------
namespace ChatGPT.Web.Helpers;

/// <summary>
/// Claims Transformation Module
/// </summary>
public class MyClaimsTransformation : IClaimsTransformation
{
    /// <summary>
    /// Transform the claims to add an isAdmin claim and an Admin role membership
    /// </summary>
    public Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
    {
        var claimsIdentity = new ClaimsIdentity();
        if (principal != null && principal.Identity != null)
        {
            claimsIdentity = new ClaimsIdentity(principal.Identity);

            // <hack> Yes... this is a hack, but it works for this simple example. This should be replaced by actual Active Directory roles. </hack>
            var isAdmin = 
                !string.IsNullOrEmpty(Data.Constants.SuperUserFirstName) && 
                !string.IsNullOrEmpty(Data.Constants.SuperUserLastName) &&
                claimsIdentity.Name.Contains(Data.Constants.SuperUserFirstName, StringComparison.InvariantCultureIgnoreCase) && 
                claimsIdentity.Name.Contains(Data.Constants.SuperUserLastName, StringComparison.InvariantCultureIgnoreCase);

            if (isAdmin && !principal.IsInRole(Data.Constants.Security.AdminRoleName))
            {
                claimsIdentity.AddClaim(new Claim(claimsIdentity.RoleClaimType, Data.Constants.Security.AdminRoleName));
            }

            //if (isAdmin && !principal.HasClaim(claim => claim.Type == Data.Constants.Security.AdminClaimType))
            //{
            //    claimsIdentity.AddClaim(new Claim(Data.Constants.Security.AdminClaimType, "true"));
            //}
        }

        principal.AddIdentity(claimsIdentity);
        return Task.FromResult(principal);
    }
}