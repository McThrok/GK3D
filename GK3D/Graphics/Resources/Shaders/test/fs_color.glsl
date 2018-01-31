#version 330

struct Light {
 vec3 position;
 vec3 color;
 float ambientIntensity;
 float diffuseIntensity;

 int type;
 vec3 direction;
 float coneAngle;

};

in vec3 v_norm;
in vec3 FragPos;
in vec4 color;
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
outputColor = vec4(0,0,0,1);
	vec3 n = normalize(v_norm);

	for(int i = 0; i < 30; i++){

  // Skip lights with no effect
  if(lights[i].color == vec3(0,0,0))
  {
   continue;
  }
  
  vec3 lightvec = normalize(lights[i].position - FragPos);

  // Check spotlight angle
  bool inCone = false;
  if(lights[i].type == 1 && degrees(acos(dot(lightvec, lights[i].direction))) < lights[i].coneAngle)
  {
   inCone = true;
  }

  // Directional lighting
  if(lights[i].type == 2){
   lightvec = lights[i].direction;
  }

  // Colors
  vec4 light_ambient = lights[i].ambientIntensity * vec4(lights[i].color, 0.0);
  vec4 light_diffuse = lights[i].diffuseIntensity * vec4(lights[i].color, 0.0);

  // Ambient lighting
  outputColor = outputColor + color * light_ambient * vec4(material_ambient, 0.0);

  // Diffuse lighting
  float lambertmaterial_diffuse = max(dot(n, lightvec), 0.0);

  // Spotlight, limit light to specific angle
  if(lights[i].type != 1 || inCone){
   outputColor = outputColor + (light_diffuse * color * vec4(material_diffuse, 0.0)) * lambertmaterial_diffuse;
  }

  // Specular lighting
  vec3 reflectionvec = normalize(reflect(-lightvec, v_norm));
  vec3 viewvec = normalize(vec3(inverse(view) * vec4(0,0,0,1)) - FragPos); 
  float material_specularreflection = max(dot(v_norm, lightvec), 0.0) * pow(max(dot(reflectionvec, viewvec), 0.0), material_specExponent);

  // Spotlight, specular reflections are also limited by angle
  if(lights[i].type != 1 || inCone){
   outputColor = outputColor + vec4(material_specular * lights[i].color, 0.0) * material_specularreflection;
  }
 }
}