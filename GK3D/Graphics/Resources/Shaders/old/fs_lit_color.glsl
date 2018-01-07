#version 330

in vec3 FragPos;  
in vec3 v_norm;
in vec4 color;

out vec4 FragColor;

uniform vec3 light_position;
uniform vec3 light_color;

uniform vec3 viewPos; 

void main()
{
	vec3 lightColor = light_color;
	vec3 lightPos = light_position;

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
    vec3 viewDir = normalize(viewPos - FragPos);
    vec3 reflectDir = reflect(-lightDir, norm);  
    float spec = pow(max(dot(viewDir, reflectDir), 0.0), 32);
    vec3 specular = specularStrength * spec * lightColor;  
        
    vec3 result = (ambient + diffuse + specular) * color;
    FragColor = vec4(result, 1.0);
}