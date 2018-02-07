#version 330

// Holds information about a light
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

in vec3 v_norm;
in vec3 v_pos;
in vec4 v_color;
out vec4 outputColor;

uniform mat4 view;

uniform vec3 material_ambient;
uniform vec3 material_diffuse;
uniform vec3 material_specular;
uniform float material_specExponent;

uniform Light lights[30];

void
main()
{
	outputColor = vec4(0, 0, 0, 1);
	vec3 n = normalize(v_norm);

	for (int i = 0; i < 30; i++) {

		if (lights[i].color == vec3(0, 0, 0)) continue;

		vec3 lightvec = normalize(lights[i].position - v_pos);
		if (lights[i].type == 2)
			lightvec = lights[i].direction;


		//ambient lighting
		vec4 ambient = lights[i].ambientIntensity * vec4(material_ambient, 0.0) * vec4(lights[i].color, 0.0);

		//spotlight
		bool inCone = false;
		float cone_factor = 0;

		if (lights[i].type == 1 && degrees(acos(dot(lightvec, lights[i].direction))) < lights[i].coneAngle)
		{
			cone_factor = pow(dot(lightvec, lights[i].direction), 16);
			inCone = true;
		}


		//diffuse lighting
		vec4 diffuse = vec4(0, 0, 0, 1);
		float lambertmaterial_diffuse = max(dot(n, lightvec), 0.0);

		if (lights[i].type != 1 || inCone)
			diffuse = lights[i].diffuseIntensity * vec4(material_diffuse, 0.0) * vec4(lights[i].color, 0.0) * lambertmaterial_diffuse;

		if (inCone)
			diffuse *= cone_factor;


		//specular lighting
		vec3 reflectionvec = normalize(reflect(-lightvec, v_norm));
		vec3 viewvec = normalize(vec3(inverse(view) * vec4(0, 0, 0, 1)) - v_pos);
		float material_specularreflection = max(dot(v_norm, lightvec), 0.0) * pow(max(dot(reflectionvec, viewvec), 0.0), material_specExponent);

		vec4 specular = vec4(0, 0, 0, 1);
		if (lights[i].type != 1 || inCone) {
			specular = vec4(material_specular * lights[i].color, 0.0) * material_specularreflection;
		}

		if (inCone)
			specular *= cone_factor;

		outputColor += (ambient + diffuse + specular)* v_color;
	}

}