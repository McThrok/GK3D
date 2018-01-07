#version 330

in vec3 vPosition;
in vec3 vColor;  
in vec3 vNormal;

out vec3 v_norm;
out vec3 v_pos;
out vec4 color;

uniform mat4 modelviewproj;
uniform mat4 model;
uniform mat4 view;

void main()
{
	gl_Position = modelviewproj * vec4(vPosition, 1.0);
	color = vec4( vColor, 1.0);

	mat3 normMatrix = transpose(inverse(mat3(model)));
	v_norm = normMatrix * vNormal;
	v_pos = (model * vec4(vPosition, 1.0)).xyz;
}