public class GetBodyOnResponse : ActionFilterAttribute
{

    public override void OnActionExecuted(  HttpActionExecutedContext actionExecutedContext)
    {            
        if (actionExecutedContext.Response.IsSuccessStatusCode && actionExecutedContext.Request.Method.ToString() == "POST")             
        {
            var actName = actionExecutedContext.ActionContext.ActionDescriptor.ActionName;

            // Here is the body
            Dictionary<string,object> body = GetBodyFromRequest(actionExecutedContext);               
        }
    }

    public Dictionary<string,object> GetBodyFromRequest(HttpActionExecutedContext context)
    {
        string data;
        Dictionary<string, object> datas = new Dictionary<string, object>();
        using (var stream = context.Request.Content.ReadAsStreamAsync().Result)
        {
            if (stream.CanSeek)
            {
                stream.Position = 0;
            }
            data = context.Request.Content.ReadAsStringAsync().Result;

            string[] elements = data.Split(new char[] {'&'});

            foreach (string elem in elements ) 
            {
                string[] el = elem.Split(new char[] { '=' });
                datas.Add(el[0],el[1]);
            }
        }
        return datas;
    }
}

/* I didn´t find a function to get a body's structured form, but always there is another ways  */