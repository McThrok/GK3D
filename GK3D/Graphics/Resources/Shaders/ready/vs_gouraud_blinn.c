#version 330

struct Light {
	vec3 position;
	vec3 color;
	float ambientIntensity;
	float diffuseIntensity;

	int type;
	vec3 direction;
	float coneAngle;
	float coneExponent;
};

in vec3 vPosition;
in vec3 vColor;
in vec3 vNormal;
out vec4 v_color;

uniform mat4 projection;
uniform mat4 model;
uniform mat4 view;

uniform vec3 material_ambient;
uniform vec3 material_diffuse;
uniform vec3 material_specular;
uniform float material_specExponent;

uniform Light lights[30];

void main()
{
	gl_Position = projection * view * model * vec4(vPosition, 1.0);

	mat3 normMatrix = transpose(inverse(mat3(model)));
	vec3 n = normalize(normMatrix * vNormal);
	vec3 pos = (model * vec4(vPosition, 1.0)).xyz;

	v_color = vec4(0, 0, 0, 1);

	for (int i = 0; i < 30; i++) {

		if (lights[i].color == vec3(0, 0, 0)) continue;

		//vector to light
		vec3 lightvec = normalize(lights[i].position - pos);
		if (lights[i].type == 2)
			lightvec = lights[i].direction;


		//spotlight
		float cone_factor = 0;

		if (lights[i].type == 1 && degrees(acos(dot(lightvec, lights[i].direction))) < lights[i].coneAngle)
			cone_factor = pow(max(dot(lightvec, lights[i].direction), 0.0), 16);


		//ambient lighting
		vec4 ambient = lights[i].ambientIntensity * vec4(material_ambient, 0.0) * vec4(lights[i].color, 0.0);


		//diffuse lighting
		float lambertmaterial_diffuse = max(dot(n, lightvec), 0.0);
		vec4 diffuse = lights[i].diffuseIntensity * vec4(material_diffuse, 0.0) * vec4(lights[i].color, 0.0) * lambertmaterial_diffuse;

		if (lights[i].type == 1)
			diffuse *= cone_factor;


		//specular lighting
		vec3 viewvec = normalize(vec3(inverse(view) * vec4(0, 0, 0, 1)) - pos);
		vec3 blinnH = normalize(lightvec + viewvec);
		float material_specularreflection = pow(max(dot(blinnH, n), 0.0), material_specExponent);
		vec4 specular = vec4(material_specular * lights[i].color, 0.0) * material_specularreflection;

		if (lights[i].type == 1)
			specular *= cone_factor;

		v_color += (ambient + diffuse + specular)* vec4(vColor, 1);
	}
}