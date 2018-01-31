#version 330

struct DirLight {
    vec3 direction;
	
    vec3 diffuse;
};

in vec4 color;
in vec3 FragPos;  
in vec3 v_norm;

out vec4 outputColor;

//uniform vec3 light_position;
//uniform vec3 light_color;
//uniform Light lights[30];
uniform DirLight test[30];
 
uniform vec3 viewPos; 
void
main()
{
vec3 a = test[0].direction;

vec3 light_position =  vec3(1.0,1.0,1.0);
vec3 light_color  =  vec3(1.0,1.0,1.0);
	// ambient
    float ambientStrength = 0.2f;
    vec3 ambient = ambientStrength * light_color;

	//diffuse
    vec3 norm = normalize(v_norm);
    vec3 lightDir = normalize(light_position - FragPos);
    float diff = max(dot(norm, lightDir), 0.0);
    vec3 diffuse = diff * light_color;
	

    // specular
    float specularStrength = 0.2f;
    vec3 viewDir = normalize(viewPos - FragPos);
    vec3 reflectDir = reflect(-lightDir, norm);  
    float spec = pow(max(dot(viewDir, reflectDir), 0.0), 32);
    vec3 specular = specularStrength * spec * light_color;  
	
    vec3 result = (ambient + diffuse + specular) * color;
    outputColor = vec4(result, 1.0);
}