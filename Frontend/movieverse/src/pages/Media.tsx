import { useEffect } from "react";
import { useParams } from "react-router-dom";

export const Media: React.FC = () => {
	const params = useParams();

	useEffect(() => {
		console.log(params);
	}, []);

	return <h1>Media</h1>
}