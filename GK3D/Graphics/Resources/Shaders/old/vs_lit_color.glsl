#version 330

in vec3 vPosition;
in vec3 vNormal;
in  vec3 vColor;

out vec4 color;
out vec3 FragPos;
out vec3 v_norm;

uniform mat4 modelviewproj;
uniform mat4 model;
uniform mat4 view;


void main()
{
	FragPos = vec3(model * vec4(vPosition, 1.0));
    v_norm = mat3(transpose(inverse(model))) * vNormal;  
    color = vec4( vColor, 1.0);
	gl_Position = modelviewproj * vec4(vPosition, 1.0);
}