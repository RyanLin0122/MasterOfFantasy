
public static partial class JSONTemplates {

	/*
	 * Vector2
	 */
	public static Vector2 ToVector2(JSONObject obj) {
		float x = obj["x"] ? obj["x"].f : 0;
		float y = obj["y"] ? obj["y"].f : 0;
		return new Vector2(x, y);
	}
	public static JSONObject FromVector2(Vector2 v) {
		JSONObject vdata = JSONObject.obj;
		if(v.x != 0)	vdata.AddField("x", v.x);
		if(v.y != 0)	vdata.AddField("y", v.y);
		return vdata;
	}

}
