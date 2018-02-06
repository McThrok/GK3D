#version 330

in vec3 v_norm;
in vec3 v_pos;
in vec4 color;
out vec4 outputColor;

//light
uniform vec3 light_position_0;
uniform vec3 light_direction_0;
uniform vec3 light_color_0;

uniform vec3 light_position_1;
uniform vec3 light_direction_1;
uniform vec3 light_color_1;

uniform vec3 light_position_2;
uniform vec3 light_direction_2;
uniform vec3 light_color_2;

uniform vec3 viewPos;

vec4 getColor(vec3 light_position, vec3 light_color, vec3 light_direction) {

	light_direction = normalize(light_direction);
	vec3 norm = normalize(v_norm);

	vec3 dirToLight = normalize(light_position - v_pos);
	float spotlight = pow(max(dot(dirToLight, light_direction), 0.0), 8);


	float ambientStrength = 0.05f;
	vec3 ambient = ambientStrength * light_color;

	float diff = max(dot(norm, dirToLight), 0.0);
	vec3 diffuse = diff * light_color * spotlight;

	float specularStrength = 0.2f;
	vec3 viewDir = normalize(viewPos - v_pos);
	vec3 blinnH = normalize(viewDir + dirToLight);
	float spec = pow(max(dot(viewDir, blinnH), 0.0), 32);
	vec3 specular = specularStrength * spec * light_color * spotlight;

	vec4 colorToAdd = vec4((ambient + diffuse + specular), 1)* color;

	return colorToAdd;
}

void main()
{
	outputColor = vec4(0, 0, 0, 1);
	outputColor += getColor(light_position_0, light_color_0, light_direction_0);
	outputColor += getColor(light_position_1, light_color_1, light_direction_1);
	outputColor += getColor(light_position_2, light_color_2, light_direction_2);
}