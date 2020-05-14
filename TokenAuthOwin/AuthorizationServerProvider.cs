public class AuthorizationServerProvider : OAuthAuthorizationServerProvider
    {
        Sesion sesion = new Sesion();
        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {   
            // get the parameters before invoke the context.Validated and store the values in an auxiliar object.
            sesion.RFC = context.Parameters.GetValues("First Parameter").FirstOrDefault();
            sesion.Usuario = context.Parameters.GetValues("Second parameter").FirstOrDefault();
            sesion.Password = context.Parameters.GetValues("Third parameter").FirstOrDefault();
            context.Validated();
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            new Autenticacion().IniciarSesion(sesion);
            if (!sesion.isAuth)
            {
                context.SetError("invalid_grant", sesion.result);
                return;
            }

            var identity = new ClaimsIdentity(context.Options.AuthenticationType);
            identity.AddClaim(new Claim("Rol", sesion.Rol));
            identity.AddClaim(new Claim("User", sesion.Usuario));
            identity.AddClaim(new Claim("Password", new Encriptaciones().getMD5(sesion.Password)));
            identity.AddClaim(new Claim("RFC", sesion.RFC));

            context.Validated(identity);
        }
        
    }