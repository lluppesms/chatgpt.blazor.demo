//-----------------------------------------------------------------------
// <copyright file="MyClaimsTransformation.cs" company="Luppes Consulting, Inc.">
// Copyright 2023, Luppes Consulting, Inc. All rights reserved.
// </copyright>
// <summary>
// Claims Transformation Module
// </summary>
//-----------------------------------------------------------------------
namespace chatgpt.blazor.Helpers;

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
            var isAdmin = claimsIdentity.Name.Contains(data.Constants.Security.SuperUserFirstName, StringComparison.InvariantCultureIgnoreCase) && claimsIdentity.Name.Contains(data.Constants.Security.SuperUserLastName, StringComparison.InvariantCultureIgnoreCase);

            if (isAdmin && !principal.IsInRole(data.Constants.Security.AdminRoleName))
            {
                claimsIdentity.AddClaim(new Claim(claimsIdentity.RoleClaimType, data.Constants.Security.AdminRoleName));
            }

            //if (isAdmin && !principal.HasClaim(claim => claim.Type == data.Constants.Security.AdminClaimType))
            //{
            //    claimsIdentity.AddClaim(new Claim(data.Constants.Security.AdminClaimType, "true"));
            //}
        }

        principal.AddIdentity(claimsIdentity);
        return Task.FromResult(principal);
    }
}