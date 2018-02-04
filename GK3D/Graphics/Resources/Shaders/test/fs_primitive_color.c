#version 330

in vec3 v_norm;
in vec3 v_pos;
in vec4 color;
out vec4 outputColor;

//light1
uniform vec3 light_position_0;
uniform vec3 light_color_0;

uniform mat4 view;

vec4 getColor(vec3 light_position, vec3 light_color) {


	vec3 lightColor = light_color;
	vec3 lightPos = light_position;
	vec3 FragPos = v_pos;

	// ambient
	float ambientStrength = 0.2f;
	vec3 ambient = ambientStrength * lightColor;

	// diffuse 
	vec3 norm = normalize(v_norm);
	vec3 lightDir = normalize(lightPos - FragPos);
	float diff = max(dot(norm, lightDir), 0.0);
	vec3 diffuse = diff * lightColor;

	// specular
    float specularStrength = 0.2f;
    vec3 viewDir = normalize(lightPos - v_pos);
    vec3 reflectDir = reflect(-lightDir, norm);  
    float spec = pow(max(dot(viewDir, reflectDir), 0.0), 32);
    vec3 specular = specularStrength * spec * lightColor;  

	vec4 colorToAdd = vec4((ambient + diffuse + specular), 1)* color ;

	return colorToAdd;
}

void main()
{
	outputColor = vec4(0, 0, 0, 1);
	outputColor += getColor(light_position_0, light_color_0);
}